using System.Linq.Expressions;
using webapi.Models;

namespace webapi.Services;

public interface IUserSortService
{
    ICollection<User> GetUsersFromRole(ICollection<User> users, UserSortService.Role role);
    ICollection<User> GetUsersFromSort(ICollection<User> users, UserSortService.RoleSort sort);
}

public class UserSortService : IUserSortService
{
    public enum Role
    {
        Admin = 0,
        User = 1
    } 

    public enum RoleSort
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

    public ICollection<User> GetUsersFromSort(ICollection<User> users, RoleSort sort)
    {
        IQueryable<User> query = users.AsQueryable();

        foreach (var include in SortSwitch(sort))
        {
            query = query.Where(include);
        }

        return query.ToArray();
    }

    private Expression<Func<User, bool>>[]? SortSwitch(RoleSort sort)
    {
        switch (sort)
        {
            case RoleSort.All:
                return [c => c.Id != null];

            case RoleSort.Non_admins:
                return [c => c.UserRole == 1];

            case RoleSort.Shareholders:
                return [c => c.UserRole == 1 || c.UserRole == 0];

            case RoleSort.Admins:
                return [c => c.UserRole == 0];

            case RoleSort.Over_Admins:
                return [c => c.UserRole == 0];

            default:
                return [];
        }
    }
}
