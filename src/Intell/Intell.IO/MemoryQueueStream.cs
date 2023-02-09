using System;
using System.IO;

namespace Intell.IO {

    ///<summary>Creates a stream whose backing store is memory that allow writing and reading as queue.</summary>
    public class MemoryQueueStream {
        private byte[] _buffer;    // allocated internally only.
        private int readPosition;  // read head.
        private int writePosition; // write head.

        ///<summary>Initializes a new instance of the <see cref="MemoryQueueStream"/> class with an expandable capacity initialized to 512.</summary>
        public MemoryQueueStream() : this(512) { }

        ///<summary>Initializes a new instance of the <see cref="MemoryQueueStream"/> class with an expandable capacity initialized as specified.</summary>
        public MemoryQueueStream(int capacity) { _buffer = new byte[capacity]; }

        ///<summary>Gets the number of bytes allocated for this stream.</summary>
        public int Capacity {
            get { return _buffer.Length; }
        }

        ///<summary>Gets the number of bytes available to read for this stream.</summary>
        public int Available {
            get { return writePosition - readPosition; }
        }

        ///<summary>Gets current read position.</summary>
        public int ReadPosition {
            get { return readPosition; }
        }

        ///<summary>Gets current write position.</summary>
        public int WritePosition {
            get { return writePosition; }
        }

        ///<summary>Gets private buffer.</summary>
        public byte[] GetBuffer() { return _buffer; }

        ///<summary>Writes a byte array to the underlying stream.</summary>
        ///<param name="buffer">A byte array containing the data to write.</param>
        public void Write(byte[] buffer) { Write(buffer, 0, buffer.Length); }

        ///<summary>Writes a region of a byte array to the current stream.</summary>
        ///<param name="buffer">A byte array containing the data to write.</param>
        ///<param name="offset">The index of the first byte to read from buffer and to write to the stream.</param>
        ///<param name="count">The number of bytes to read from buffer and to write to the stream.</param>
        public void Write(byte[] buffer, int offset, int count) {
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (offset < 0) throw new ArgumentOutOfRangeException("offset");
            if (count < 0) throw new ArgumentOutOfRangeException("count");

            if (count + offset > buffer.Length)
                throw new ArgumentException("Offset and count were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");

            //Read position
            //↓
            //a b _ _ _ _  
            //0 1 2 3 4 5
            //    ↑
            //    Write position

            EnsureCapacity(count);
            Buffer.BlockCopy(buffer, offset, _buffer, writePosition, count);

            writePosition += count;
        }

        ///<summary>Reads the specified number of bytes from the stream, starting from a specified point in the byte array.</summary>
        ///<param name="buffer">The buffer to read data into.</param>
        ///<param name="offset">The starting point in the buffer at which to begin reading into the buffer.</param>
        ///<param name="count">The number of bytes to read.</param>
        ///<returns>The number of bytes read into buffer. This might be less than the number of bytes requested if that many bytes are not available, or it might be zero if the end of the stream is reached.</returns>
        public int Read(byte[] buffer, int offset, int count) {
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (offset < 0) throw new ArgumentOutOfRangeException("offset");
            if (count < 0) throw new ArgumentOutOfRangeException("count");

            if (count + offset > buffer.Length)
                throw new ArgumentException("Offset and count were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");


            //  Read position
            //  ↓
            //a b c d e f g 0 0 0 0  0  0  0  0  0
            //0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15
            //              ↑
            //              Write position

            int available = writePosition - readPosition;

            if (available > count) available = count;

            //0 1 2 3


            // writePosition-readPosition
            Buffer.BlockCopy(_buffer, readPosition, buffer, offset, available);
            readPosition += available;
            return available;
        }

        ///<summary>Reads the next byte from the current stream and advances the current position of the stream by one byte.</summary>
        public byte ReadByte() {
            int available = Available;
            if (available == 0) throw new EndOfStreamException();

            var value = _buffer[readPosition];
            readPosition++;

            return value;
        }

        ///<summary>Reads a 4-byte signed from the current stream and advances the current position of the stream by four byte.</summary>
        public int ReadInt() {
            int available = Available;
            if (available < 4) throw new EndOfStreamException();

            var value = BitConverter.ToInt32(_buffer, readPosition);
            readPosition += 4;

            return value;
        }


        ///<summary>Ensure buffer have enough space for new data.</summary>
        private void EnsureCapacity(int count) {
            //  ↓ Read position
            //a b c d e f g h i j
            //0 1 2 3 4 5 6 7 8 9 10 11
            //                    ↑ Write position

            if (writePosition == readPosition) writePosition = readPosition = 0;

            var _capacity = _buffer.Length;
            var _available = writePosition - readPosition;

            if (_available + count > _capacity) {
                // must create a new buffer
                IncreaseBuffer(_available + count);
            } else {
                if (writePosition + count < _capacity) {
                    // ready to write
                } else {
                    // must relocate
                    // if user writes 1 mega bytes, then read 1 byte, then write 1 byte
                    // this will make us relocate 1 mega to put 1 byte??

                    if (count > _available) {
                        // mean count is bigger 50% of capacity;
                        Relocation();
                    } else {
                        // should create a new buffer for performance
                        IncreaseBuffer(_available + count);
                    }

                }
            }

        }
        ///<summary>Recreate new buffer for specified capacity and copy data to the new buffer.</summary>
        private void IncreaseBuffer(int newCapacity) {

            var _capacity = _buffer.Length;
            var _available = writePosition - readPosition;

            if (newCapacity < 512) newCapacity = 512; // 512
            if (newCapacity < _capacity * 2) newCapacity = _capacity * 2;


            byte[] newBuffer = new byte[newCapacity];


            Buffer.BlockCopy(_buffer, readPosition, newBuffer, 0, _available);

            _buffer = newBuffer;
            readPosition = 0;
            writePosition = _available;
        }
        ///<summary>Move data of buffer to zero-index.</summary>
        private void Relocation() {
            if (writePosition == readPosition) { writePosition = readPosition = 0; return; }

            //    ↓ Read position
            //a b c d e f g h i j
            //0 1 2 3 4 5 6 7 8 9 10 11
            //                    ↑ Write position

            Buffer.BlockCopy(_buffer, readPosition, _buffer, 0, writePosition - readPosition);
            writePosition -= readPosition;
            readPosition = 0;

        }
    }
}
