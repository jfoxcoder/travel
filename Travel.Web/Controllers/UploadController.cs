using Backload;
using Backload.Controllers;
using Backload.Eventing.Args;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Travel.Web.Controllers
{

    public class UploadController : Controller
    {
        public UploadController()
        {

        }
      
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }


        [Route("Upload/UploadHandler", Name = "UploadEndpoint_old")]
        public async Task<ActionResult> UploadHandler()
        {
            FileUploadHandler handler = new FileUploadHandler(Request, this);
            handler.IncomingRequestStarted += handler_IncomingRequestStarted;

            ActionResult result = await handler.HandleRequestAsync();
            return result;
        }

        void handler_IncomingRequestStarted(object sender, IncomingRequestEventArgs e)
        {
            throw new Exception("handler_IncomingRequestStarted");
            // Demo: Disallow PUT request within the event handler.
            //if (e.Context.HttpMethod == "PUT") e.Context.PipelineControl.ExecutePipeline = false;

            int destinationId;
            if (! int.TryParse(e.Param.BackloadValues.ObjectContext, out destinationId))
            {
                // todo: handle error
                
            }


            
        }


        

       

        //// Since version 1.9 you can call the asynchronous handler method 
        //public async Task<ActionResult> FileHandler()
        //{
        // //   throw new Exception("FileHandler()");
        //    var handler = new FileUploadHandler(Request, this);

        //    handler.DeleteFilesRequestStarted += handler_DeleteFilesRequestStarted;





        //    // Call base class method to handle the file upload asynchronously
        //    ActionResult result = await base.HandleRequestAsync();
        //    return result;
        //}

        //void handler_DeleteFilesRequestStarted(object sender, DeleteFilesRequestEventArgs e)
        //{
        //  //  throw new Exception("handler_DeleteFilesRequestStarted");
        //    e.Context.Request.Params.AllKeys
        //                            .ToList()
        //                            .ForEach(k => Debug.WriteLine("key = " + k));
        //}

        //public override Task<ActionResult> UploadHandler()
        //{
        // //   throw new Exception("UploadHandler()");

        //    return base.UploadHandler();
        //}
    }
}