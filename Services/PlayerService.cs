using footic.DTOs.player;
using footic.EData;
using footic.Models;
using Microsoft.EntityFrameworkCore;

namespace footic.Services
{
   
    
       


            public class PlayerService : IplayerService
    {
                private readonly PlSimulationDbContext _context;

                public PlayerService(PlSimulationDbContext context)
                {
                    _context = context;
                }

                // 1. جلب كل لاعبي فريق معين
                public async Task<List<PlayerDTO>> GetPlayersByTeamIdAsync(int teamId)
                {
                    return await _context.Players
                        .Include(p => p.PlayerStat)
                        .Where(p => p.TeamId == teamId)
                        .AsNoTracking()
                        .Select(p => MapToDTO(p)) // استخدمنا ميثود المابينج لتقليل تكرار الكود
                        .ToListAsync();
                }

                // 2. جلب بيانات لاعب محدد بواسطة الـ ID
                public async Task<PlayerDTO> GetPlayerByIdAsync(int playerId)
                {
                    var player = await _context.Players
                        .Include(p => p.PlayerStat)
                        .Include(p => p.Team) // ضفنا التيم هنا عشان نعرف بيلعب فين
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.PlayerId == playerId);

                    if (player == null) return null;

                    return MapToDetailsDTO(player);
                }

                // ميثود "سينيور" خاصة لتوحيد عملية تحويل الموديل لـ DTO (Refactoring)
                private static PlayerDTO MapToDTO(Player p)
                {
                    return new PlayerDTO
                    {
                        playerid=p.PlayerId,
                        ShirtNumber=p.Pnumber,
                        PlayerImage=p.Pimage,
                        name=p.Pname,
                        Age=p.Age,
                        nationality=p.Nationality,
                        Position=p.Position,
                        MarketValue=(decimal)p.MarketValue,
                        StrongFoot=p.StrongFoot,
                        power=p.Poweer,
                    };
                }
                private static PlayerStatsDTO MapToDetailsDTO(Player p)
                {
                    return new PlayerStatsDTO
                    {
                        playerid = p.PlayerId,
                        ShirtNumber = p.Pnumber,
                        PlayerImage = p.Pimage,
                        name = p.Pname,
                        Age = p.Age,
                        nationality = p.Nationality,
                        Position = p.Position,
                        MarketValue = (decimal)p.MarketValue,
                        StrongFoot = p.StrongFoot,
                        power = p.Poweer,
                        Joined=p.Joined,
                        EndContract=p.EndContract,
                        Fit=p.Fit,
                        Height=p.PlayerStat.Height,
                        Weight=p.PlayerStat.Weight,
                        Goals=p.PlayerStat.Goals,
                        Assists=p.PlayerStat.Assists,
                        RedCards=p.PlayerStat.RedCards,
                        YellowCards=p.PlayerStat.YellowCards,
                    };
                }
            }

        }
 

