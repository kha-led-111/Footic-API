using footic.DTOs.Match;
using footic.EData;
using footic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace footic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly MatchService _matchservice;
        private readonly PlSimulationDbContext _context;
        private readonly CreateMatchesService _creatematchservice;

        public MatchController(MatchService matchservice, PlSimulationDbContext context, CreateMatchesService creatematchservice )
        {
            _matchservice = matchservice;
            _context = context;
            _creatematchservice = creatematchservice;
        }
        [HttpGet("all matches")]
        public async Task<ActionResult<List<MatchDTO>>> GetAll()
        {

            var matches = await _matchservice.GetallMatches();

            if (matches == null || !matches.Any())
            {
                return Ok(new List<MatchDTO>());
            }

            return Ok(matches);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MatchDTO>> GetById(int id)
        {
            var match = await _matchservice.GetMatchById(id);

            if (match == null)
            {
                return NotFound(new { message = $"Match with ID {id} was not found." });
            }

            return Ok(match);



        }
        [HttpGet("preview-schedule")]
        public async Task<IActionResult> PreviewSchedule()
        {
            // تحميل بيانات الفرق والملاعب أولاً
            await _creatematchservice.InitializeData();

            // توليد الجدول
            var schedule = _creatematchservice.Schedule();

            if (schedule == null || !schedule.Any())
            {
                return BadRequest("لم يتم توليد أي مباريات. تحقق من إعدادات الموسم والفرق.");
            }

            // عرض الماتشات مرتبة حسب الأسبوع
            var preview = schedule.OrderBy(m => m.week).ThenBy(m => m.Matchdate)
                                            .ThenBy(m => m.Matchtime)
                                            .ToList();

            return Ok(preview);
        }
        [HttpPost("confirm-save")]
        public async Task<IActionResult> ConfirmSave([FromBody] List<displayMatchDto> matchesDto)
        {
            if (matchesDto == null || !matchesDto.Any())
            {
                return BadRequest("قائمة المباريات فارغة، لا يوجد شيء لحفظه.");
            }

            try
            {
                int savedCount = await _creatematchservice.SaveGeneratedMatches(matchesDto);
                return Ok(new { message = $"تم حفظ {savedCount} مباراة بنجاح في قاعدة البيانات." });
            }
            catch (Exception ex)
            {
                // دي هتمسك أي خطأ زي الـ ForeignKey لو الـ ID مش موجود
                return StatusCode(500, $"حدث خطأ أثناء الحفظ: {ex.Message}");
            }
        }


    }
}
