using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ncfe.CodeTest;

namespace Test
{
    [TestClass]
    public class GetLearnerFromFailoverLearnerDataAccessTests
    {
        private LearnerService learnerService;

        [TestInitialize]
        public void Setup()
        {
            learnerService = new LearnerService();

            //TEST DATA - Add 99 FailoverEntries with a DateTime set in the last 10 mins
            //TEST DATA - Add a Learner with an Id of 1 to FailoverLearnerDataAccess
        }

        [TestMethod]
        //isLearnerArchived = false
        //IsFailoverModeEnabled = true
        //100 FailoverEntries exist
        //LearnerResponse.IsArchived = false
        public void Test001_AssertNotNull()
        {
            ConfigurationManager.AppSettings.Set("IsFailoverModeEnabled", "true");
            //TEST DATA - Add 1 FailoverEntry with a DateTime set in the last 10 mins

            var result = learnerService
                .GetLearner(1, false);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        //isLearnerArchived = true
        //IsFailoverModeEnabled = true
        //100 FailoverEntries exist
        //LearnerResponse.IsArchived = false
        public void Test002_AssertNull()
        {
            ConfigurationManager.AppSettings.Set("IsFailoverModeEnabled", "true");
            //TEST DATA - Add 1 FailoverEntry with a DateTime set in the last 10 mins

            var result = learnerService
                .GetLearner(1, true);

            Assert.IsNull(result);
        }

        [TestMethod]
        //isLearnerArchived = false
        //IsFailoverModeEnabled = false
        //100 FailoverEntries exist
        //LearnerResponse.IsArchived = false
        public void Test003_AssertNull()
        {
            ConfigurationManager.AppSettings.Set("IsFailoverModeEnabled", "false");
            //TEST DATA - Add 1 FailoverEntry with a DateTime set in the last 10 mins

            var result = learnerService
                .GetLearner(1, false);

            Assert.IsNull(result);
        }

        [TestMethod]
        //isLearnerArchived = false
        //IsFailoverModeEnabled = true
        //100 FailoverEntries do not exist
        //LearnerResponse.IsArchived = false
        public void Test004_AssertNull()
        {
            ConfigurationManager.AppSettings.Set("IsFailoverModeEnabled", "true");

            var result = learnerService
                .GetLearner(1, false);

            Assert.IsNull(result);
        }

        [TestMethod]
        //isLearnerArchived = false
        //IsFailoverModeEnabled = true
        //100 FailoverEntries exist
        //LearnerResponse.IsArchived = true
        public void Test005_AssertNull()
        {
            ConfigurationManager.AppSettings.Set("IsFailoverModeEnabled", "true");
            //TEST DATA - Add 1 FailoverEntry with a DateTime set in the last 10 mins
            //TEST DATA - Set Learner(Id:1) Archived = true

            var result = learnerService
                .GetLearner(1, false);

            Assert.IsNull(result);
        }
    }
}