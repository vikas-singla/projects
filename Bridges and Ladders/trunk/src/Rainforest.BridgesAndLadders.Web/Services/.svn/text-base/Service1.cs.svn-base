using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.Mvc;
using Rainforest.BridgesAndLadders.Data;

namespace Rainforest.BridgesAndLadders.Web
{
	// Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page
	// NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
	// a single instance of the service to process all calls.	

    // http://msdn.microsoft.com/en-us/magazine/dd943053.aspx
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerCall)]
    // NOTE: If the service is renamed, remember to update the global.asax.cs file
    public class Service1
	{
		// TODO: Implement the collection resource that will contain the SampleItem instances

		[WebGet(UriTemplate="")]
        
        // NOTE: using Authorize attribute instead of PrincipalPermission so that we can redirect the user as opposed to throwing an exception.
        // The WebServiceHostFactory class has been overidden to respect the Authorize attribute that's normally only looked at during actions.
        [Authorize(Roles="Administrators")]
		public List<SampleItem> GetCollection()
		{
			// TODO: Replace the current implementation to return a collection of SampleItem instances
			return new List<SampleItem>() { new SampleItem() { Id = 1, StringValue = "Hello" } };
		}

		[WebInvoke(UriTemplate = "", Method = "POST")]
		public SampleItem Create(SampleItem instance)
		{
			// TODO: Add the new instance of SampleItem to the collection
			throw new NotImplementedException();
		}

		[WebGet(UriTemplate = "{id}")]
		public SampleItem Get(string id)
		{
			// TODO: Return the instance of SampleItem with the given id
			throw new NotImplementedException();
		}

		[WebInvoke(UriTemplate = "{id}", Method = "PUT")]
		public SampleItem Update(string id, SampleItem instance)
		{
			// TODO: Update the given instance of SampleItem in the collection
			throw new NotImplementedException();
		}

		[WebInvoke(UriTemplate = "{id}", Method = "DELETE")]
		public void Delete(string id)
		{
			// TODO: Remove the instance of SampleItem with the given id from the collection
			throw new NotImplementedException();
        }

        #region helper methods



        #endregion

    }
}
