using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Messenger.Core
{
    public class FramesHutch
    {
        public List<byte[]> Frames { get; } = new();
        public int MaxAmount => _maxAmount;

        private int _maxAmount = 1;

        public byte[] GetBase64Frame(WriteableBitmap sourceBitmap)
            => sourceBitmap is null ? throw new ArgumentNullException("SourceBitmap is null.") 
                                    : sourceBitmap.ToBitmapImage()
                                                  .ToByteArray("JPG");

        public bool Add(byte[] frameBase64)
        {
            if (Frames.Count() == _maxAmount)
            {
                return false;
            }

            Frames.Add(frameBase64);
            return true;
        }

        public void Clear()
        {
            Frames.Clear();
        }

        public bool IsFull()
            => Frames.Count() == _maxAmount;
    }
}
