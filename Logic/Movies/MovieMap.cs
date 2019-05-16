using FluentNHibernate;
using FluentNHibernate.Mapping;

namespace Logic.Movies
{
    public class MovieMap : ClassMap<Movie>
    {
        public MovieMap()
        {
            Id(x => x.Id);

            DiscriminateSubClassesOnColumn("licensingModel");

            Map(x => x.Name);
            Map(Reveal.Member<Movie>("LicensingModel")).CustomType<int>();
        }
    }
}