using UnityEngine;
using System;
using GoogleMobileAds.Api;
using System.Collections.Generic;
using GoogleMobileAds.Common;

[Serializable]
public class IdInfo
{
    public string BannerAdId = "ca-app-pub-3940256099942544/6300978111";
    //public string BigBannerId = "ca-app-pub-3940256099942544/6300978111";
    public string InterstitialAdId = "ca-app-pub-3940256099942544/1033173712";
    public string _AppOpenAdId = "ca-app-pub-3940256099942544/9257395921";
    public string _RewardedAdId = "ca-app-pub-3940256099942544/5224354917";
    //public string _RewardedInterAdId = "ca-app-pub-3940256099942544/5224354917";

}
public class AdsManager : MonoBehaviour
{
    public IdInfo AllIdsData;

    private BannerView _bannerView;
    private BannerView _bigBannerView;
    private InterstitialAd _interstitialAd;
    private RewardedInterstitialAd _rewardedInterstitialAd;
    private RewardedAd _rewardedAd;

    string testRewardedInterId = "ca-app-pub-3940256099942544/5224354917";
    string testRewardedId = "ca-app-pub-3940256099942544/5224354917";
    string testBannerAdId = "ca-app-pub-3940256099942544/6300978111";
    string testBigBannerAdId = "ca-app-pub-3940256099942544/6300978111";
    string testInterstitialAdId = "ca-app-pub-3940256099942544/1033173712";
    private string testAppOpenAdId = "ca-app-pub-3940256099942544/9257395921";

    [Header("Banner Settings")]
    public AdPosition BannerAdPosition = AdPosition.Top;

    [Header("Banner Settings")]
    public AdPosition BannerBigAdPosition = AdPosition.Top;

    public bool bShowBannerAtStart = false;
    public bool showTestAds = false;
    [HideInInspector]
    public bool canStartGame = false;

    public delegate void OnRewardSuccess();
    public static event OnRewardSuccess OnRewardGiven;

    public static AdsManager instance = null;


    public delegate void RewardUserDelegate();
    private static RewardUserDelegate NotifyReward;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(this);
        }

    }
    private void OnDestroy()
    {
        // Always unlisten to events when complete.
        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
    }


    // Start is called before the first frame update
    void Start()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //Change the Text
            canStartGame = true;
        }
        else
        {
            MobileAds.Initialize((initStatus) =>
            {
                ELogs("Admob Initialized");
                if (bShowBannerAtStart)
                    RequestBanner();

                LoadInterstitialAd();
                LoadAppOnenAd();
                //LoadRewardedInterstitialAd();
                LoadRewardAd();
            });
        }
    }

    public void REDDOG()
    {
        PlayerPrefs.SetInt("rrr", 1);
        PlayerPrefs.Save();
        CloseBannerAd();
    }

    public bool GetRedDog()
    {
        if (PlayerPrefs.GetInt("rrr", 0) == 1)
            return true;
        else
            return false;
    }
    #region BannerMethods
    public void RequestBanner()
    {
        if (GetRedDog())
            return;

        string adUnitId = AllIdsData.BannerAdId;

        if (showTestAds)
            adUnitId = testBannerAdId;


        // Clean up banner ad before creating a new one.
        if (_bannerView != null)
        {
            _bannerView.Destroy();
        }

        AdSize adaptiveSize =
                AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        _bannerView = new BannerView(adUnitId, adaptiveSize, BannerAdPosition);

        // Register for ad events.
        _bannerView.OnBannerAdLoaded += _bannerView_OnBannerAdLoaded;
        _bannerView.OnBannerAdLoadFailed += OnBannerAdLoadFailed;

        AdRequest adRequest = new AdRequest();

        // Load a banner ad.
        _bannerView.LoadAd(adRequest);

    }

    public void CloseBannerAd()
    {

        if (_bannerView != null)
        {
            ELogs("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    #endregion

    #region BigBannerMethod
    //public void RequestBigBanner()
    //{
    //    if (GetRedDog())
    //        return;

    //    string adUnitId = AllIdsData.BigBannerId;

    //    if (showTestAds)
    //        adUnitId = testBigBannerAdId;


    //    // Clean up banner ad before creating a new one.
    //    if (_bigBannerView != null)
    //    {
    //        _bigBannerView.Destroy();
    //    }

    //    AdSize BigBannerAd =
    //            AdSize.MediumRectangle;

    //    _bigBannerView = new BannerView(adUnitId, BigBannerAd, BannerBigAdPosition);

    //    // Register for ad events.
    //    _bigBannerView.OnBannerAdLoaded += _bannerView_OnBannerAdLoaded;
    //    _bigBannerView.OnBannerAdLoadFailed += OnBannerAdLoadFailed;

    //    AdRequest adRequest = new AdRequest();

    //    // Load a banner ad.
    //    _bigBannerView.LoadAd(adRequest);

    //}

    //public void CloseBigBannerAd()
    //{
    //    if (_bigBannerView != null)
    //    {
    //        ELogs("Destroying Bigbanner view.");
    //        _bigBannerView.Destroy();
    //        _bigBannerView = null;
    //    }
    //}
    #endregion

    #region Banner callback handlers

    private void _bannerView_OnBannerAdLoaded()
    {
        ELogs("Banner Shown");
        //throw new NotImplementedException();
    }

    //private void OnBannerAdLoaded(object sender, EventArgs args)
    //{
    //    Debug.Log("Banner view loaded an ad with response : "
    //             + _bannerView.GetResponseInfo());
    //    Debug.Log("Ad Height: "+_bannerView.GetHeightInPixels() + ", width: "+ _bannerView.GetWidthInPixels());

    //}

    private void OnBannerAdLoadFailed(LoadAdError error)
    {
        Debug.LogError("Banner view failed to load an ad with error : "
                + error);
    }

    #endregion

    #region InterStitialMethods
    public void LoadInterstitialAd()
    {
        //AnalyticsImp.Instance.LogEvent("Interstitial_Requested");

        if (GetRedDog())
            return;

        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            DestroyInterAd();
        }

        string _adUnitId = AllIdsData.InterstitialAdId;

        if (showTestAds)
            _adUnitId = testInterstitialAdId;


        ELogs("Loading interstitial ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // Send the request to load the ad.
        InterstitialAd.Load(_adUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            // If the operation failed with a reason.
            if (error != null)
            {
                //AnalyticsImp.Instance.LogEvent("Interstitial_FailToLoad");

                Debug.LogError("Interstitial ad failed to load an ad with error : " + error);
                return;
            }
            // If the operation failed for unknown reasons.
            // This is an unexpected error, please report this bug if it happens.
            if (ad == null)
            {
                //AnalyticsImp.Instance.LogEvent("Interstitial_NullAd");
                Debug.LogError("Unexpected error: Interstitial load event fired with null ad and null error.");
                return;
            }

            // The operation completed successfully.
            //AnalyticsImp.Instance.LogEvent("Interstitial_Loaded");
            ELogs("Interstitial ad loaded with response : " + ad.GetResponseInfo());
            _interstitialAd = ad;

            // Register to ad events to extend functionality.
            RegisterEventHandlers(ad);

            // Inform the UI that the ad is ready.
            // AdLoadedStatus?.SetActive(true);
        });
    }

    /// <summary>
    /// Shows the ad.
    /// </summary>
    public void ShowInterstitialAd()
    {
        if (GetRedDog())
            return;

        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            ELogs("Showing interstitial ad.");
            //AnalyticsImp.Instance.LogEvent("Interstitial_Shown");

            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }

        // Inform the UI that the ad is not ready.
        //AdLoadedStatus?.SetActive(false);
    }

    /// <summary>
    /// Destroys the ad.
    /// </summary>
    public void DestroyInterAd()
    {
        if (_interstitialAd != null)
        {
            ELogs("Destroying interstitial ad.");
            //AnalyticsImp.Instance.LogEvent("Interstitial_Destroy");
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        // Inform the UI that the ad is not ready.
        //AdLoadedStatus?.SetActive(false);
    }

    /// <summary>
    /// Logs the ResponseInfo.
    /// </summary>
    public void LogResponseInfo()
    {
        if (_interstitialAd != null)
        {
            var responseInfo = _interstitialAd.GetResponseInfo();
            UnityEngine.Debug.Log(responseInfo);
        }
    }

    private void RegisterEventHandlers(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            ELogs(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            //AnalyticsImp.Instance.LogEvent("Interstitial_ImpressionRecorded");
            ELogs("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            ELogs("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            ELogs("Interstitial ad full screen content opened.");

        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            ELogs("Interstitial ad full screen content closed.");
            LoadInterstitialAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            //AnalyticsImp.Instance.LogEvent("Interstitial_ContentFailed");
            Debug.LogError("Interstitial ad failed to open full screen content with error : "
                + error);

            if (Time.time - lastCalledTime > delayInSeconds)
            {
                LoadInterstitialAd();

                // Update the last called time
                lastCalledTime = Time.time;
            }

        };
    }
    private float lastCalledTime = 0f;
    private float delayInSeconds = 3f;
    #endregion

    #region AppOpenMethods
    // App open ads can be preloaded for up to 4 hours.
    private readonly TimeSpan TIMEOUT = TimeSpan.FromHours(4);
    private DateTime _expireTime;
    private AppOpenAd _appOpenAd;

    public void LoadAppOnenAd()
    {
        //AnalyticsImp.Instance.LogEvent("AppOpen_Requested");

        //if (!RemoteConfigScript.Instance.BoolData.CanShowAppOpen)
        //{
        //    canStartGame = true;
        //    return;

        //}
        // Clean up the old ad before loading a new one.
        if (_appOpenAd != null)
        {
            DestroyAppOpenAd();
        }

        if (GetRedDog())
            return;

        string _adUnitId = AllIdsData._AppOpenAdId;

        if (showTestAds)
            _adUnitId = testAppOpenAdId;


        ELogs("Loading app open ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // Send the request to load the ad.
        AppOpenAd.Load(_adUnitId, adRequest, (AppOpenAd ad, LoadAdError error) =>
        {
            // If the operation failed with a reason.
            if (error != null)
            {
                canStartGame = true;
                //AnalyticsImp.Instance.LogEvent("AppOpen_FailToLoad");

                Debug.LogError("App open ad failed to load an ad with error : "
                                + error);
                return;
            }

            // If the operation failed for unknown reasons.
            // This is an unexpected error, please report this bug if it happens.
            if (ad == null)
            {
                canStartGame = true;
                //AnalyticsImp.Instance.LogEvent("AppOpen_NullAd");

                Debug.LogError("Unexpected error: App open ad load event fired with " +
                               " null ad and null error.");
                return;
            }

            // The operation completed successfully.
            ELogs("App open ad loaded with response : ");
            //AnalyticsImp.Instance.LogEvent("AppOpen_Loaded");
            _appOpenAd = ad;

            // App open ads can be preloaded for up to 4 hours.
            _expireTime = DateTime.Now + TIMEOUT;

            // Register to ad events to extend functionality.
            RegisterEventHandlers(ad);

            if (isFirstOpen)
            {
                isFirstOpen = false;
                // This is used to launch the loaded ad when we open the app.
                AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
#if UNITY_IOS
                ShowAppOpenAd();
#endif

            }
            // Inform the UI that the ad is ready.

        });
    }
    public static bool isFirstOpen = true;
    /// <summary>
    /// Shows the ad.
    /// </summary>
    public void ShowAppOpenAd()
    {
        if (GetRedDog())
            return;

        // App open ads can be preloaded for up to 4 hours.
        if (_appOpenAd != null && _appOpenAd.CanShowAd() && DateTime.Now < _expireTime)
        {
            ELogs("Showing app open ad.");
            //AnalyticsImp.Instance.LogEvent("AppOpen_Shown");

            _appOpenAd.Show();
        }
        else
        {
            Debug.LogError("App open ad is not ready yet.");
        }

        // Inform the UI that the ad is not ready.

    }

    /// <summary>
    /// Destroys the ad.
    /// </summary>
    public void DestroyAppOpenAd()
    {
        if (_appOpenAd != null)
        {
            ELogs("Destroying app open ad.");
            //AnalyticsImp.Instance.LogEvent("AppOpen_Destroying");
            _appOpenAd.Destroy();
            _appOpenAd = null;
        }

        // Inform the UI that the ad is not ready.

    }


    private void OnAppStateChanged(AppState state)
    {
        ELogs("App State changed to : " + state);

        // If the app is Foregrounded and the ad is available, show it.
        if (state == AppState.Foreground)
        {
            Invoke("ShowAppOpenAd", 1);
        }
    }

    private void RegisterEventHandlers(AppOpenAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            ELogs(String.Format("App open ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            //AnalyticsImp.Instance.LogEvent("AppOpen_ImpressionRecoreded");

            ELogs("App open ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            ELogs("App open ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            //AnalyticsImp.Instance.LogEvent("AppOpen_Opened");

            ELogs("App open ad full screen content opened.");

            // Inform the UI that the ad is consumed and not ready.

        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            ELogs("App open ad full screen content closed.");
            canStartGame = true;
            // It may be useful to load a new ad when the current one is complete.
            LoadAppOnenAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            //AnalyticsImp.Instance.LogEvent("AppOpen_ContentFailed");
            ELogs("App open ad failed to open full screen content with error : "
                            + error);
            canStartGame = true;
        };
    }
    #endregion

    #region RewardAD
    public void LoadRewardAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            DestroyRewardAd();
        }

        string _adUnitId = AllIdsData._RewardedAdId;

        if (showTestAds)
            _adUnitId = testRewardedId;

        Debug.Log("Loading rewarded ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // Send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            // If the operation failed with a reason.
            if (error != null)
            {
                Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                return;
            }
            // If the operation failed for unknown reasons.
            // This is an unexpected error, please report this bug if it happens.
            if (ad == null)
            {
                Debug.LogError("Unexpected error: Rewarded load event fired with null ad and null error.");
                return;
            }

            // The operation completed successfully.
            Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());

            _rewardedAd = ad;

            // Register to ad events to extend functionality.
            RewardRegisterEventHandlers(ad);

            // Inform the UI that the ad is ready.

        });
    }

    /// <summary>
    /// Shows the ad.
    /// </summary>
    public void ShowRewardAd(string RewardName, int n)
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            Debug.Log("Showing rewarded ad.");
            _rewardedAd.Show((Reward reward) =>
            {
                //AnalyticsImp.Instance.LogEvent("RV_Watched");
                //AnalyticsImp.Instance.LogEvent("RV_" + RewardName);
                Debug.Log(String.Format("Rewarded ad granted a reward: {0} {1}",
                                        reward.Amount,
                                        reward.Type));
            });
        }
        else
        {
            Debug.LogError("Rewarded ad is not ready yet.");
        }

        // Inform the UI that the ad is not ready.

    }
    public void ShowRewardAdWithDelegate(RewardUserDelegate NotifyRV)
    {
        NotifyReward = NotifyRV;

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            Debug.Log("Showing rewarded ad.");


            _rewardedAd.Show((Reward reward) =>
            {

                NotifyReward();
                //AnalyticsImp.Instance.LogEvent("RV_Watched");
                //AnalyticsImp.Instance.LogEvent("RV_" + RewardName);
                Debug.Log(String.Format("Rewarded ad granted a reward: {0} {1}",
                                        reward.Amount,
                                        reward.Type));
            });
        }
        else
        {
            Debug.LogError("Rewarded ad is not ready yet.");
         
        }

        // Inform the UI that the ad is not ready.

    }



    /// <summary>
    /// Destroys the ad.
    /// </summary>
    public void DestroyRewardAd()
    {
        if (_rewardedAd != null)
        {
            Debug.Log("Destroying rewarded ad.");
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        // Inform the UI that the ad is not ready.

    }

    /// <summary>
    /// Logs the ResponseInfo.
    /// </summary>
    public void LogRewardResponseInfo()
    {
        if (_rewardedAd != null)
        {
            var responseInfo = _rewardedAd.GetResponseInfo();
            UnityEngine.Debug.Log(responseInfo);
        }
    }

    private void RewardRegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
            LoadRewardAd();
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when the ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            LoadRewardAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            LoadRewardAd();
            Debug.LogError("Rewarded ad failed to open full screen content with error : "
                + error);
        };
    }
    #endregion

    #region RewardInter
    //public void LoadRewardedInterstitialAd()
    //{
    //    // Clean up the old ad before loading a new one.
    //    if (_rewardedInterstitialAd != null)
    //    {
    //        _rewardedInterstitialAd.Destroy();
    //        _rewardedInterstitialAd = null;
    //    }

    //    string _adUnitId = AllIdsData._RewardedInterAdId;

    //    if (showTestAds)
    //        _adUnitId = testRewardedId;

    //    Debug.Log("Loading the rewarded interstitial ad.");

    //    // create our request used to load the ad.
    //    var adRequest = new AdRequest();
    //    adRequest.Keywords.Add("unity-admob-sample");

    //    // send the request to load the ad.
    //    RewardedInterstitialAd.Load(_adUnitId, adRequest,
    //        (RewardedInterstitialAd ad, LoadAdError error) =>
    //        {
    //            // if error is not null, the load request failed.
    //            if (error != null || ad == null)
    //            {
    //                Debug.LogError("rewarded interstitial ad failed to load an ad " +
    //                               "with error : " + error);
    //                return;
    //            }

    //            Debug.Log("Rewarded interstitial ad loaded with response : "
    //                      + ad.GetResponseInfo());

    //            _rewardedInterstitialAd = ad;
    //            RewardedInterstitialRegisterEventHandlers(ad);
    //        });
    //}
    public void ShowRewardedInterstitialAd(RewardUserDelegate NotifyRV)
    {
        const string rewardMsg =
            "Rewarded interstitial ad rewarded the user. Type: {0}, amount: {1}.";
        NotifyReward = NotifyRV;
        if (_rewardedInterstitialAd != null && _rewardedInterstitialAd.CanShowAd())
        {
            _rewardedInterstitialAd.Show((Reward reward) =>
            {
                NotifyReward();
                //AnalyticsImp.Instance.LogEvent("IR_Watched");
                //AnalyticsImp.Instance.LogEvent("IR_" + RewardName);
                // TODO: Reward the user.
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }
    private void RewardedInterstitialRegisterEventHandlers(RewardedInterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            //LoadRewardedInterstitialAd();
            Debug.Log("Rewarded interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            //LoadRewardedInterstitialAd();
            Debug.LogError("Rewarded interstitial ad failed to open " +
                           "full screen content with error : " + error);
        };
    }
    #endregion
    void ELogs(string log)
    {
        Debug.Log("$$ " + log);
    }
}
