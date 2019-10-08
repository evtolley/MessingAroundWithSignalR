using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace SignalRTestz.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private IHubContext<NotifyHub, ITypedHubClient> _createHubContext;
        private IHubContext<UpdateHub, ITypedHubClient> _updateHubContext;
        private IHubContext<DeleteHub, ITypedHubClient> _deleteHubContext;
        private readonly IMessageRepository _messageRepo;

        public MessageController(
            IHubContext<NotifyHub, ITypedHubClient> createHubContext,
            IHubContext<UpdateHub, ITypedHubClient> updateHubContext,
            IHubContext<DeleteHub, ITypedHubClient> deleteHubContext,
            IMessageRepository messageRepo)
        {
            _createHubContext = createHubContext;
            _updateHubContext = updateHubContext;
            _deleteHubContext = deleteHubContext;
            _messageRepo = messageRepo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_messageRepo.GetMessages().Reverse());
        }

        [HttpPost]
        [Route("create")]
        public IActionResult AddMessage([FromBody]Message msg)
        {
            string retMessage = string.Empty;

            try
            {
                msg.DateTimeString = DateTime.Now.ToString("f");
                _messageRepo.AddMessage(msg);
                _createHubContext.Clients.All.BroadcastMessage("Success", msg);
                retMessage = "Success";
            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }

            return Ok(new { message = retMessage});
        }

        [HttpPost]
        [Route("update")]
        public IActionResult UpdateMessage([FromBody]Message msg)
        {
            string retMessage = string.Empty;

            try
            {
                _messageRepo.UpdateMessage(msg);
                _updateHubContext.Clients.All.BroadcastMessage("Success", msg);
                retMessage = "Success";
            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }

            return Ok(new { message = retMessage });
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult DeleteMessage([FromBody]Message msg)
        {
            string retMessage = string.Empty;

            try
            {
                _messageRepo.DeleteMessage(msg.Id);
                _deleteHubContext.Clients.All.BroadcastMessage("Success", msg);
                retMessage = "Success";
            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }

            return Ok(new { message = retMessage });
        }
    }
}