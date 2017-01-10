package com.example.admin.servicechattm.model;

import com.android.volley.*;
import com.android.volley.toolbox.JsonObjectRequest;
import com.example.admin.servicechattm.R;
import com.example.admin.servicechattm.ServiceChatApp;
import org.json.JSONException;
import org.json.JSONObject;

public class LoginRequest{

    private static final String LOGIN_REQUEST_URL = "/Login";

    public static final String ERROR_CONNECTION = "Connection";
    public static final String ERROR_UNKNOWN = "Unknown";
    public static final String ERROR_INVALID_CREDENTIALS = "InvalidCredentials";
    public static final String ERROR_USER_NOT_FOUND = "UserNotFound";
    public static final String ERROR_INVALID_PASSWORD = "InvalidPassword";

    public interface ResponseListener {
        void onSuccess(User user);
        void onError(String error);
    }

    public static JsonObjectRequest create(String username, String password, final ResponseListener listener) {
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
                    try{
                        if (response.getBoolean("Success")){
                            listener.onSuccess(User.fromJsonResponse(response));
                        } else {
                            listener.onError(response.getString("Error"));
                        }
                    } catch (JSONException e){
                        listener.onError(ERROR_UNKNOWN);
                    }
                }
            },
            new Response.ErrorListener() {
                @Override
                public void onErrorResponse(VolleyError error){
                    NetworkResponse response = error.networkResponse;
                    // TODO: Handle status code
                    listener.onError(ERROR_CONNECTION);
                }
            }
        );
    }
}
