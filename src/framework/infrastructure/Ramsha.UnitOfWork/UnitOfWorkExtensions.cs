namespace Ramsha.UnitOfWork;

public static class UnitOfWorkExtensions
{
    public static bool IsReservedFor(this IUnitOfWork unitOfWork, string reservationName)
    {
        return unitOfWork.IsReserved && unitOfWork.ReservationName == reservationName;
    }
}
