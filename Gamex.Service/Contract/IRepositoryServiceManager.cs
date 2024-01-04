﻿namespace Gamex.Service.Contract
{
    public interface IRepositoryServiceManager
    {
        ITournamentService TournamentService { get; }
        IJWTHelper JWTHelper { get; }
    }
}