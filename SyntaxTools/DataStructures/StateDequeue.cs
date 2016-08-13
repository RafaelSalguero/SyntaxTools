using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTools.DataStructures
{
    /// <summary>
    /// A queue that its created from a read only collection and that have only the dequeue operation. Its current read pointer can be pushed onto an state stack in order to make local dequeue operations
    /// </summary>
    public class StateDequeue<T>
    {
        /// <summary>
        /// Create a new StateDequeue from a read only list
        /// </summary>
        /// <param name="Data"></param>
        public StateDequeue(IReadOnlyList<T> Data)
        {
            this.Data = Data;
            this.readPointer = 0;
        }
        private IReadOnlyList<T> Data;
        private int readPointer;
        /// <summary>
        /// Gets the absolute current read pointer
        /// </summary>
        public int AbsoluteReadPointer
        {
            get
            {
                return readPointer;
            }
        }
        private Stack<int> state = new Stack<int>();

        /// <summary>
        /// Push the current read pointer to the state stack
        /// </summary>
        public void PushState()
        {
            state.Push(readPointer);
        }

        /// <summary>
        /// Pop and assign the read pointer from the state stack
        /// </summary>
        public void PopState()
        {
            readPointer = state.Pop();
        }

        /// <summary>
        /// Pop an state from the stack without assigning it
        /// </summary>
        public void DropState()
        {
            state.Pop();
        }

        /// <summary>
        /// Gets weather the queue is empty
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return readPointer >= Data.Count;
            }
        }

        /// <summary>
        /// Read the next element without consuming it
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            return Data[readPointer];
        }

        /// <summary>
        /// Consume an element from the queue
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            if (readPointer < Data.Count)
                return Data[readPointer++];
            else
                throw new InvalidOperationException("The queue is empty");
        }
    }
}
