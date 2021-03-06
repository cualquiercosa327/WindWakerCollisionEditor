﻿using GameFormatReader.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WindWakerCollisionEditor
{
    public class Category : INotifyPropertyChanged
    {
        #region NotifyPropertyChanged overhead

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private static int m_nameAddInt;

        public event EventHandler<UndoRedoEventArgs> UndoRedoCommandEventArgs;

        public string Name
        {
            get { return m_name; }
            set
            {
                if (value != m_name)
                {
                    UndoRedoEventArgs args = new UndoRedoEventArgs();

                    args.cmd = new NameUndoRedo(m_name, value, this);

                    OnNameChange(args);

                    m_name = value;

                    NotifyPropertyChanged();
                }
            }
        }

        private string m_name;

        public void SetName(string name)
        {
            m_name = name;

            NotifyPropertyChanged("Name");
        }

        public BindingList<Group> Groups
        {
            get { return m_groups; }
            set
            {
                if (value != m_groups)
                {
                    m_groups = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private BindingList<Group> m_groups;

        public int InitialIndex;

        public Category()
        {
            m_name = "NewCategory" + m_nameAddInt++;

            m_groups = new BindingList<Group>();
        }

        public Category(string name)
        {
            m_name = name;

            m_groups = new BindingList<Group>();
        }

        public Category(EndianBinaryReader stream)
        {
            m_groups = new BindingList<Group>();

            int streamStart = (int)stream.BaseStream.Position;

            stream.BaseStream.Position = stream.ReadInt32();

            char[] tempChars = Encoding.ASCII.GetChars(stream.ReadBytesUntil(0));

            m_name = new string(tempChars);

            stream.BaseStream.Position = streamStart + 0x34;
        }

        public TerrainType Terrain
        {
            get { return m_terrain; }
            set
            {
                if (value != m_terrain)
                {
                    m_terrain = value;

                    NotifyPropertyChanged();
                }
            }
        }

        private TerrainType m_terrain;

        private int m_roomNumber;

        public int RoomNumber
        {
            get { return m_roomNumber; }
            set
            {
                if (value != m_roomNumber)
                {
                    m_roomNumber = value;

                    NotifyPropertyChanged();
                }
            }
        }

        protected virtual void OnNameChange(UndoRedoEventArgs e)
        {
            EventHandler<UndoRedoEventArgs> handler = UndoRedoCommandEventArgs;

            if (handler != null)
                handler(this, e);
        }
    }
}
