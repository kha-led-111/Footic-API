using footic.DTOs.player;

namespace footic.EData
{
    public interface IplayerService
    {
        Task<List<PlayerDTO>> GetPlayersByTeamIdAsync(int teamId);
        Task<PlayerDTO> GetPlayerByIdAsync(int playerId);
    }
}
