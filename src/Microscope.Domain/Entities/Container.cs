using Microscope.Domain.SharedKernel;

namespace Microscope.Domain.Entities
{
    public class Container : IAggregateRoot<string>
    {
        #region ctor

        protected Container()
        {
            
        }

        protected Container(string name): this()
        {
            this.Id = name;
        }

        public static Container NewContainer(string name)
        {
            return new Container(name);
        }

        #endregion

        #region properties

        public string Id { get; private set; }
        public string Name () => Id;

        #endregion
    }
}
