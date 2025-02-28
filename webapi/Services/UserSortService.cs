using System.Linq.Expressions;
using webapi.Models;

namespace webapi.Services;

public interface IUserSortService
{
    ICollection<User> GetUsersFromRole(ICollection<User> users, UserSortService.Role role);
    ICollection<User> GetUsersFromSort(ICollection<User> users, UserSortService.RoleGroup sort);
}

public class UserSortService : IUserSortService
{
    public enum Role
    {
        Admin = 0,
        User = 1
    } 

    public enum RoleGroup
    {
        All = 0,
        Non_admins = 1,
        Shareholders = 2,
        Admins = 3,
        Over_Admins = 4
    }

    public ICollection<User> GetUsersFromRole(ICollection<User> users, Role role)
    {
        IQueryable<User> query = users.AsQueryable();

        query = query.Where(c => role.Equals(c.UserRole));

        return query.ToArray();
    }

    public ICollection<User> GetUsersFromSort(ICollection<User> users, RoleGroup sort)
    {
        IQueryable<User> query = users.AsQueryable();

        foreach (var include in SortSwitch(sort))
        {
            query = query.Where(include);
        }

        return query.ToArray();
    }

    private Expression<Func<User, bool>>[]? SortSwitch(RoleGroup sort)
    {
        switch (sort)
        {
            case RoleGroup.All:
                return [c => c.Id != null];

            case RoleGroup.Non_admins:
                return [c => c.UserRole == 1];

            case RoleGroup.Shareholders:
                return [c => c.UserRole == 1 || c.UserRole == 0];

            case RoleGroup.Admins:
                return [c => c.UserRole == 0];

            case RoleGroup.Over_Admins:
                return [c => c.UserRole == 0];

            default:
                return [];
        }
    }
}
