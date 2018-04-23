using DFC.Digital.Data.Interfaces;
using DFC.Digital.Repository.SitefinityCMS.Base;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Taxonomies.Model;
using Telerik.Sitefinity.Taxonomies.Web;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.UnitTests.Modules
{
    public class RelatedClassificationsRepositoryTests
    {
        private IDynamicContentExtensions fakeDynamicContentExtensions;
        private ITaxonomyManager fakeTaxonomyManager;
        private IRepository<Taxon> fakeTaxonomyRepository;
        private DynamicContent dummyContentItem;
        private ITaxonomyManagerExtensions fakeTaxonomyManagerExtensions;

        public RelatedClassificationsRepositoryTests()
        {
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>();
            fakeTaxonomyManager = A.Fake<ITaxonomyManager>();
            fakeTaxonomyRepository = A.Fake<IRepository<Taxon>>();
            dummyContentItem = A.Dummy<DynamicContent>();
            fakeTaxonomyManagerExtensions = A.Fake<ITaxonomyManagerExtensions>();
        }

        [Theory]
        [InlineData(true, "test", "test")]
        [InlineData(false, "test", "test")]
        public void GetRelatedClassificationsTest(bool classificationsAvailable, string relatedField, string taxonomyName)
        {
           //Assign
           SetupCalls(classificationsAvailable);
            var classificationRepo =
                new RelatedClassificationsRepository(fakeTaxonomyManager, fakeDynamicContentExtensions, fakeTaxonomyManagerExtensions);

            //Act
            classificationRepo.GetRelatedClassifications(dummyContentItem, relatedField, taxonomyName);

            //Assert
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<IList<Guid>>(A<DynamicContent>._, A<string>._))
                .MustHaveHappened();

          //  A.CallTo(() => fakeTaxonomyRepository.GetMany(A<Expression<Func<Taxon, bool>>>._)).MustHaveHappened();
            A.CallTo(() => fakeTaxonomyManagerExtensions.WhereQueryable(A<IQueryable<Taxon>>._, A<Expression<Func<Taxon, bool>>>._)).MustHaveHappened();
        }

        private void SetupCalls(bool classificationsAvailable)
        {
            var dummyIdList = A.CollectionOfDummy<Guid>(classificationsAvailable ? 1 : 0);
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<IList<Guid>>(A<DynamicContent>._, A<string>._))
                .Returns(dummyIdList);

            var fakeTaxonlist = A.CollectionOfDummy<Taxon>(2).ToList().AsQueryable();
            A.CallTo(() => fakeTaxonomyManager.GetTaxa<Taxon>()).Returns(fakeTaxonlist);
            A.CallTo(() => fakeTaxonomyManagerExtensions.WhereQueryable(A<IQueryable<Taxon>>._, A<Expression<Func<Taxon, bool>>>._)).Returns(fakeTaxonlist);
        }
    }
}