

namespace FalconWare.ErrorHandling
{
    public class OpResultAccessException : Exception
    {
        public OpResultAccessException() 
            : base("WasSuccess has not been checked yet - you MUST check WasSuccess before accessing Value!")
        {

        }
    }
}