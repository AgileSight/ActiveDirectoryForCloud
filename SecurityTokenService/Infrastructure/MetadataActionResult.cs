using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Protocols.WSFederation.Metadata;

namespace CloudSecurityTokenService.Infrastructure
{
    public class MetadataActionResult : ActionResult
    {
        EntityDescriptor metadata;

        public MetadataActionResult(EntityDescriptor metadata)
        {
            this.metadata = metadata;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/xml";
            
            var serializer = new MetadataSerializer();
            serializer.WriteMetadata(context.HttpContext.Response.OutputStream, metadata);

            context.HttpContext.Response.Flush();
        }
    }
}