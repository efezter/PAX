namespace PAX.TaskManager.Services.Permission
{
    public interface IPermissionService
    {
        bool HasPermission(string key);
    }
}