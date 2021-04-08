namespace Fabricdot.WebApi.Core.Filters
{
    public class ResultFilterOptions
    {
        public bool IncludeEmptyResult { get; set; }

        public ResultFilterOptions()
        {
            IncludeEmptyResult = true;
        }
    }
}