﻿namespace Ajloun_Tour.DTOs2.PackagesProgramDTOs
{
    public class PackagesProgramDTO
    {
        public int PackageProgramId { get; set; }
        public int PackageId { get; set; }
        public int ProgramId { get; set; }
        public string Name { get; set; } = null!;
        public string? ProgramTitle { get; set; }
        public int DayNumber { get; set; }
        public DateTime? ProgramDate { get; set; }
        public string? CustomTitle { get; set; }
        public string? CustomDescription { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
