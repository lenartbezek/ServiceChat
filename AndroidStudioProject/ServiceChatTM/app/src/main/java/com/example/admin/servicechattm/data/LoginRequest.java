package com.example.admin.servicechattm.data;

import android.util.Log;
import android.widget.Toast;
import com.android.volley.*;
import com.android.volley.toolbox.JsonObjectRequest;
import com.example.admin.servicechattm.Auth;
import com.example.admin.servicechattm.R;
import com.example.admin.servicechattm.ServiceChatApp;
import com.example.admin.servicechattm.model.User;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.HashMap;
import java.util.Map;

public class LoginRequest{

    private static final String LOGIN_REQUEST_URL = "/Login";

    public static final String ERROR_CONNECTION = "Connection";
    public static final String ERROR_UNKNOWN = "Unknown";
    public static final String ERROR_INVALID_CREDENTIALS = "InvalidCredentials";
    public static final String ERROR_USER_NOT_FOUND = "UserNotFound";
    public static final String ERROR_INVALID_PASSWORD = "InvalidPassword";

    public interface ResponseListener {
        void onSuccess();
        void onError(String error);
    }

    /**
     * Login z POST requestom z uporabniškim imenom in geslom v JSON formatu.
     * @param username
     * @param password
     * @param listener
     * @return Nov JsonObjectRequest.
     */
    public static JsonObjectRequest create(final String username, final String password, final ResponseListener listener) {
        String fullRequestUrl = ServiceChatApp.getContext().getResources().getString(R.string.api_base_url)+LOGIN_REQUEST_URL;
        JSONObject jsonBody = new JSONObject();
        try {
            jsonBody.put("Username", username);
            jsonBody.put("Password", password);
        } catch (JSONException e) {
            e.printStackTrace();
        }
        final String requestBody = jsonBody.toString();
        return new JsonObjectRequest(Request.Method.POST, fullRequestUrl, jsonBody,
                new Response.Listener<JSONObject>() {
                    @Override
                    public void onResponse(JSONObject response) {
                        Auth.setToken(username, password);
                        handleLoginResponse(response, listener);
                    }
                },
                new Response.ErrorListener() {
                    @Override
                    public void onErrorResponse(VolleyError error){
                        handleLoginError(error.networkResponse.statusCode, listener);
                    }
                }
        );
    }

    /**
     * Login z GET in BasicAuth tokenom.
     * @param token
     * @param listener
     * @return Nov JsonObjectRequest
     */
    public static JsonObjectRequest create(final String token, final ResponseListener listener) {
        String fullRequestUrl = ServiceChatApp.getContext().getResources().getString(R.string.api_base_url)+LOGIN_REQUEST_URL;
        return new JsonObjectRequest(Request.Method.GET, fullRequestUrl, null,
                new Response.Listener<JSONObject>() {
                    @Override
                    public void onResponse(JSONObject response) {
                        handleLoginResponse(response, listener);
                    }
                },
                new Response.ErrorListener() {
                    @Override
                    public void onErrorResponse(VolleyError error){
                        handleLoginError(error.networkResponse.statusCode, listener);
                    }
                }
        ) {
            @Override
            public Map<String, String> getHeaders() throws AuthFailureError {
                Map<String, String> map = new HashMap<>();
                map.put("Authorization", token);
                return map;
            }
        };
    }

    private static void handleLoginResponse(JSONObject response, ResponseListener listener){
        try{
            if (response.getBoolean("Success")){
                User.setMe(User.fromJsonObject(response.getJSONObject("Account")));
                listener.onSuccess();
            } else {
                listener.onError(response.getString("Error"));
            }
        } catch (JSONException e){
            listener.onError(ERROR_UNKNOWN);
        }
    }

    private static void handleLoginError(int statusCode, ResponseListener listener){
        // TODO: Switch through status codes
        listener.onError(ERROR_CONNECTION);
    }
}
