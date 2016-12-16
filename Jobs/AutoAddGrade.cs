using System;
using System.Linq;
using Quartz;
using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;

namespace com.bricksandmortarstudio.AutoAddGrade.Jobs
{
    public class AutoAddGrade : IJob
    {
        public void Execute( IJobExecutionContext context )
        {
            int thisYear = RockDateTime.Now.Year;
            int startYear = thisYear - 5;
            int endYear = thisYear - 4;
            int graduationYear = thisYear + 14;

            var startDate = new DateTime( startYear, 9, 1 );
            var endDate = new DateTime( endYear, 8, 31 );

            var recordStatusId =
                DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_RECORD_STATUS_ACTIVE.AsGuid() ).Id;
            var rockContext = new RockContext();
            var children = new PersonService( rockContext ).GetAllChildren()
                .Where( p => p.RecordStatusValueId == recordStatusId && p.GraduationYear == null && p.BirthDate >= startDate && p.BirthDate <= endDate );
            foreach ( var child in children )
            {
                child.GraduationYear = graduationYear;
            }
            rockContext.SaveChanges();
        }
    }
}
