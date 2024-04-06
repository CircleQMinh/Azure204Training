namespace MultipleAzureFunctionInSameSolution.Services
{
    public class MyService : IMyService
    {
        public string IsMyServiceWorking(){
            return "Yes, My Service is working";
        }
    }
}