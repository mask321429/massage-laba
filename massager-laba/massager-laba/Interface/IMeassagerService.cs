using massager_laba.Data.DTO;

namespace massager_laba.Interface;

public interface IMeassagerService
{
    Task<List<MessagerDTO>> GetMyMessager(Guid id);
}