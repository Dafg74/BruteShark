﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PcapProcessor
{
    class ProcessingPrecentsPredicator
    {
        public delegate void ProcessingPrecentsChangedEventHandler(object sender, ProcessingPrecentsChangedEventArgs e);
        public event ProcessingPrecentsChangedEventHandler ProcessingPrecentsChanged;

        private long _dataProcessed;
        private long _totalFilesSize;

        public long DataProcessed
        {
            get
            {
                return _dataProcessed;
            }
            set
            {
                _dataProcessed = value;
                CheckIfProcessingPrecentsChanged(_dataProcessed);
            }
        }
        public int ProcessingPrecents { get; set; }
        public HashSet<FileInfo> FilesProcessed { get; set; }

        public ProcessingPrecentsPredicator()
        {
            this.FilesProcessed = new HashSet<FileInfo>();
        }

        public ProcessingPrecentsPredicator(HashSet<FileInfo> filesInfo) : this()
        {
            _totalFilesSize = filesInfo.Sum(fi => fi.Length);
        }

        public void AddFile(FileInfo filesInfo)
        {
            _totalFilesSize += filesInfo.Length;
        }

        public void AddFiles(HashSet<FileInfo> filesInfo)
        {
            foreach (var fi in filesInfo)
            {
                AddFile(fi);
            }
        }

        public void NotifyAboutProcessedData(long additionalData)
        {
            this.DataProcessed += additionalData;
        }

        public void NotifyAboutProcessedFile(FileInfo fileProcessed)
        {
            this.FilesProcessed.Add(fileProcessed);
            this.DataProcessed = this.FilesProcessed.Sum(fi => fi.Length);
        }

        private void CheckIfProcessingPrecentsChanged(long additionalData)
        {
            if (_totalFilesSize == 0)
                return;

            var precentsAfterAddition = (int)((((decimal)additionalData / (decimal)_totalFilesSize) * 100) % 101);

            if (precentsAfterAddition > this.ProcessingPrecents)
            {
                this.ProcessingPrecents = precentsAfterAddition;

                ProcessingPrecentsChanged?.Invoke(this, new ProcessingPrecentsChangedEventArgs()
                {
                    Precents = this.ProcessingPrecents
                });
            }
        }

        public void Clear()
        {
            this._totalFilesSize = 0;
            this.DataProcessed = 0;
            this.ProcessingPrecents = 0;
            this.FilesProcessed.Clear();
        }

    }
}
