namespace Core.DTOS
{
    public class ManageRolesDTO
    {
       public string UserId {  get; set; }
       public List<RolesDTO> Roles { get; set; }
    }

    public class RolesDTO
    {
        public string RoleName { get; set; }
        public bool IsSelected {  get; set; }

    }
}
