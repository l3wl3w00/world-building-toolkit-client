#nullable enable
namespace Common
{
    public interface IQuery
    {
    }
    
    public interface IQuery<out TResult> : IQuery
    {
        TResult Get();
    }
    
    public interface IQuery<in TParam, out TResult> : IQuery
    {
        TResult Get(TParam param);
    }
    public class EmptyQuery<T> : IQuery<T>
    {
        private EmptyQuery()
        {
            
        }
        public T Get()
        {
            return default!;
        }
    }
}