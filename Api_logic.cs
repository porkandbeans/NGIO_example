using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Api_logic : MonoBehaviour
{
    private io.newgrounds.core ngio_core;
    public Text usernameText;

    // Gets called when the player is signed in.
    void onLoggedIn() 
    {
        io.newgrounds.objects.user player = ngio_core.current_user;
        //username as a string can be accessed with
        string username = player.name;
        usernameText.text = username; 

        // Since this function is called when the user is DEFINITELY logged in, it's a good idea to call stuff related to
        // the API here. For instance, unlocking medals, submitting high-scores, switching to the next scene if you want to do that...
        SceneManager.LoadScene("Scene 2");
    }

    // Gets called if there was a problem with the login (expired sessions, server problems, etc).
    void onLoginFailed() 
    {
        io.newgrounds.objects.error error = ngio_core.login_error;
        Debug.Log(error); // maybe you wanna see why it failed?
        usernameText.text = "Failed to log in!";
    }

    // When the user clicks your log-in button
    public void requestLogin() 
    {
        // This opens passport and tells the core what to do when a definitive result comes back.
        ngio_core.requestLogin(onLoggedIn, onLoginFailed, onLoginCancelled);
    }

    /*
     * You should have a 'cancel login' button in your game and have it call this, just to be safe.
     * If the user simply closes the browser tab rather than clicking a button to cancel, we won't be able to detect that.
     */
    void cancelLogin() 
    {
        /*
         * This will call onLoginCancelled because you already added it as a callback via ngio_core.requestLogin()
         * for ngio_core.onLoginCancelled()
         */ 
        ngio_core.cancelLoginRequest();
    }

    // Check if the user has a saved login when your game starts
    void Start() 
    {
        
        ngio_core = GetComponent<io.newgrounds.core>();
        // Do this after the core has been initialized
        ngio_core.onReady(() => { 

            // Call the server to check login status
            ngio_core.checkLogin((bool logged_in) => {

                if (logged_in) 
                {
                    onLoggedIn();
                }
            });
        });
    }

    // And finally, have your 'sign out' button call this
    void logOut() 
    {
        ngio_core.logOut();
    }

    void onLoginCancelled() => Debug.Log("Login was cancelled...");
}
