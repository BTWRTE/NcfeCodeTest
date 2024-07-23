using System;
using System.Configuration;
using System.Linq;

namespace Ncfe.CodeTest
{
    public class LearnerService
    {
        public Learner GetLearner(int learnerId, bool isLearnerArchived)
        {
            LearnerResponse learnerResponse = null;

            if (!isLearnerArchived)
            {
                //Failover mode logic moved to private method
                if (IsInFailoverMode())
                {
                    learnerResponse = FailoverLearnerDataAccess.GetLearnerById(learnerId);
                }
                else
                {
                    learnerResponse = new LearnerDataAccess().LoadLearner(learnerId);
                }
            }

            //Rearranged logic to use one call to GetArchivedLearner for both use cases
            if (isLearnerArchived || learnerResponse.IsArchived)
            {
                return new ArchivedDataService().GetArchivedLearner(learnerId);
            }
            else
            {
                return learnerResponse.Learner;
            }
        }

        private bool IsInFailoverMode()
        {
            //No need to query DB if the setting is not true
            if (bool.Parse(ConfigurationManager.AppSettings["IsFailoverModeEnabled"]))
            {
                var failoverTimeStart = DateTime.Now.AddMinutes(-10);

                var failedRequestCount = new FailoverRepository()
                    .GetFailOverEntries()
                    .Where(foe => foe.DateTime > failoverTimeStart)
                    .Count();

                return failedRequestCount > 100;
            }

            return false;
        }
    }
}