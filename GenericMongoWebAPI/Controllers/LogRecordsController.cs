using GenericMongoWebAPI.IRepository;
using GenericMongoWebAPI.Model;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GenericMongoWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogRecordsController : ControllerBase
    {
        private readonly ILogRecordRepository _repository = null;

        public LogRecordsController(ILogRecordRepository _repository)
        {
            this._repository = _repository;
        }

        #region ---GET-----------------------------------------------------------------------
        // GET: api/LogRecords
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<LogRecord>? logRecords;
            try
            {
                logRecords = _repository.GetAll(); //.Where(x => x.DeletedFlag == false);
                if (logRecords.Count() == 0) return StatusCode(StatusCodes.Status404NotFound, "No data found");
                return Ok(logRecords);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
            finally { logRecords = null; }
        }

        // GET: api/LogRecords/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            LogRecord? logRecord;
            try
            {
                logRecord = _repository.Get(id);
                if (logRecord == null) return StatusCode(StatusCodes.Status404NotFound, "LogRecord with id " + id.ToString() + " not found");
                return Ok(logRecord);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
            finally { logRecord = null; }
        }
        #endregion

        #region ---POST----------------------------------------------------------------------
        [HttpPost]
        public IActionResult Post([FromBody] LogRecord logRecord)
        {
            try
            {
                _repository.Save(logRecord);
                return Created(new Uri(Request.GetDisplayUrl() + logRecord.Id.ToString()), logRecord);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally { logRecord = null; }
        }
        #endregion

        #region ---PUT-----------------------------------------------------------------------
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] LogRecord logRecord)
        {
            try
            {
                var entity = _repository.Get(id);
                logRecord.Id = entity.Id;
                if (entity == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "LogRecord with id " + id.ToString() + " not found");
                }
                else
                {
                    //logRecord.LogTimeStamp = logRecord.LogTimeStamp == null ? DateTime.Now.ToString() : logRecord.LogTimeStamp;
                    _repository.Save(logRecord);
                    return Ok(logRecord);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally { logRecord = null; }
        }
        #endregion

        #region ---DELETE--------------------------------------------------------------------
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                var entity = _repository.Get(id);
                if (entity == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "LogRecord with id " + id.ToString() + " not found");
                }
                else
                {
                    var message = _repository.Delete(id);
                    return Ok(message);
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        #endregion
    }
}