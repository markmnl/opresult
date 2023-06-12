using System;

namespace FalconWare.ErrorHandling
{
    /// <summary>
    /// <see cref="OpResult{TResult}.Value"/> was attempted to be accessed before <see cref="OpResult{TResult}.WasSuccess"/> was checked.
    /// </summary>
    public class OpResultAccessException : Exception
    {
        public OpResultAccessException()
            : base("WasSuccess has not been checked yet - you MUST check WasSuccess before accessing Value!")
        {

        }
    }
}