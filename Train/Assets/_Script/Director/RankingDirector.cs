using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using System.Threading.Tasks;
using Unity.Services.Leaderboards.Exceptions;
using Steamworks;
using System;

public class RankingDirector : MonoBehaviour
{
    [SerializeField] private string leaderboardId = "네_리더보드_ID";

    public int myBestScore;

    public Transform rankTransform;
    public GameObject rankPrefab;
    public GameObject myrankPrefab;

    public bool isSignedIn { get; private set; }

    async void Awake()
    {
        // 초기화 시작
        await Initialize();
    }

    private async Task Initialize()
    {
        // Unity Services 초기화
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            // 익명 로그인
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                isSignedIn = true;
                Debug.Log("Signed in anonymously.");
            }
            catch(Exception e)
            {
                Debug.LogWarning($"인터넷 없음 또는 로그인 실패: {e.Message}");
                isSignedIn = false;
                return;
            }
        }

        // 로그인 완료 후 내 점수 가져오기
        await FetchMyBestScore();
    }

    public async void SubmitScore(int finalScore, bool flag)
    {
        if (!isSignedIn)
        {
            return;
        }

        if (flag)
        {
            await LeaderboardsService.Instance
                .AddPlayerScoreAsync(leaderboardId, finalScore);

            myBestScore = finalScore;
        }

        FetchTopScores();
    }

    public async void FetchTopScores()
    {
        if (!isSignedIn) return;

        var res = await LeaderboardsService.Instance.GetScoresAsync(
            leaderboardId, new GetScoresOptions { Limit = 10 });

        string mySteamName = "Player"; // 기본
        try
        {
            mySteamName = SteamFriends.GetPersonaName();
        }
        catch(Exception e)
        {
            Debug.Log($"SteamWorks 미사용, 기본 이름 사용: {e.Message}");
        }
        string myPlayerID = AuthenticationService.Instance.PlayerId;
        bool isMine;

        foreach (var e in res.Results)
        {
            isMine = e.PlayerId == myPlayerID;
            Instantiate(rankPrefab, rankTransform).GetComponent<RankElement>().SetElement(e.Rank + 1, mySteamName, (int)e.Score, false, isMine);
        }

        var score = await LeaderboardsService.Instance .GetPlayerScoreAsync(leaderboardId);
        myrankPrefab.GetComponent<RankElement>().SetElement(score.Rank+1, mySteamName, (int)score.Score, true, true);
    }

    async Task FetchMyBestScore()
    {
        try
        {
            var score = await LeaderboardsService.Instance
                .GetPlayerScoreAsync(leaderboardId);

            myBestScore = (int)score.Score;
        }
        catch(LeaderboardsException e)
        {
            if (e.Reason == LeaderboardsExceptionReason.EntryNotFound)
            {
                await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, 0);

                myBestScore = 0;
            }
            else
            {
                Debug.LogError($"Leaderboard error: {e}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }
}
