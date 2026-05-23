using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Interfaces.Repositories;

using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Helpers
{
  



    public class MatchValidationHelper

    {

        private readonly IMatchRepository _matchRepository;

        private readonly IPlayerRepository _playerRepository;



        public MatchValidationHelper(

            IMatchRepository matchRepository,

            IPlayerRepository playerRepository)

        {

            _matchRepository = matchRepository;

            _playerRepository = playerRepository;

        }



        public async Task<Match> ValidateMatchForEventAsync(int matchId)

        {

            var match = await _matchRepository.GetByIdAsync(matchId);

            if (match == null)

                throw new KeyNotFoundException(

                    $"No se encontró el partido con ID {matchId}");



            if (match.Status != MatchStatus.InProgress &&

                match.Status != MatchStatus.Finished)

                throw new InvalidOperationException(

                    "Solo se pueden registrar eventos en partidos InProgress o Finished");



            return match;

        }



        public async Task<Player> ValidatePlayerInMatchAsync(

            int playerId, Match match)

        {

            var player = await _playerRepository.GetByIdAsync(playerId);

            if (player == null)

                throw new KeyNotFoundException(

                    $"No se encontró el jugador con ID {playerId}");



            if (player.TeamId != match.HomeTeamId &&

                player.TeamId != match.AwayTeamId)

                throw new InvalidOperationException(

                    "El jugador no pertenece a ninguno de los equipos del partido");



            return player;

        }



        public static void ValidateMinute(int minute)

        {

            if (minute < 1 || minute > 120)

                throw new InvalidOperationException(

                    "El minuto debe estar entre 1 y 120");

        }
        //agregamos nueva validacionm para confirmar si el equipo existe y que permite agregar alineaciones a partidos programados

        public async Task <Match> ValidateMatchForLineupAsync(int matchId)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);//buscamos por la id del partido
            if (match == null)// en caso de ser nulo, muestra mensaje de not found
                throw new KeyNotFoundException($"No se encontró el partido con ID {matchId}");

            if (match.Status != MatchStatus.Scheduled)// si no esta en agendando saca mensaje de error
                throw new InvalidOperationException($"Solo se pueden registrar alineaciones en partidos Scheduled");

            return match;//de cumplir, permite la creacion




        }
        


    }
}
