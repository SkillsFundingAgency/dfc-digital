using DFC.Digital.Data.Interfaces;
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

namespace DFC.Digital.Repository.SitefinityCMS.UnitTests
{
    public class RelatedClassificationsRepositoryTests
    {
        private IDynamicContentExtensions fakeDynamicContentExtensions;
        private ITaxonomyManager fakeTaxonomyManager;
        private DynamicContent dummyContentItem;
        private ITaxonomyManagerExtensions fakeTaxonomyManagerExtensions;

        public RelatedClassificationsRepositoryTests()
        {
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>();
            fakeTaxonomyManager = A.Fake<ITaxonomyManager>();
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

            A.CallTo(() => fakeTaxonomyManagerExtensions.WhereQueryable(A<IQueryable<FlatTaxon>>._, A<Expression<Func<FlatTaxon, bool>>>._)).MustHaveHappened();
        }

        [Theory]
        [InlineData(true, "test", "test")]
        [InlineData(false, "test", "test")]
        public void GetRelatedCmsReportClassificationsTest(bool classificationsAvailable, string relatedField, string taxonomyName)
        {
            //Assign
            SetupCalls(classificationsAvailable);
            var classificationRepo =
                new RelatedClassificationsRepository(fakeTaxonomyManager, fakeDynamicContentExtensions, fakeTaxonomyManagerExtensions);

            //Act
            classificationRepo.GetRelatedCmsReportClassifications(dummyContentItem, relatedField, taxonomyName);

            //Assert
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<IList<Guid>>(A<DynamicContent>._, A<string>._))
                .MustHaveHappened();

            A.CallTo(() => fakeTaxonomyManagerExtensions.WhereQueryable(A<IQueryable<FlatTaxon>>._, A<Expression<Func<FlatTaxon, bool>>>._)).MustHaveHappened();
        }

        private void SetupCalls(bool classificationsAvailable)
        {
            var dummyIdList = A.CollectionOfDummy<Guid>(classificationsAvailable ? 1 : 0);
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<IList<Guid>>(A<DynamicContent>._, A<string>._))
                .Returns(dummyIdList);

            var fakeTaxonlist = A.CollectionOfDummy<FlatTaxon>(2).ToList().AsQueryable();
            A.CallTo(() => fakeTaxonomyManager.GetTaxa<FlatTaxon>()).Returns(fakeTaxonlist);
            A.CallTo(() => fakeTaxonomyManagerExtensions.WhereQueryable(A<IQueryable<FlatTaxon>>._, A<Expression<Func<FlatTaxon, bool>>>._)).Returns(fakeTaxonlist);
        }
    }
}