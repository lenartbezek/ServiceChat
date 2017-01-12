package com.example.admin.servicechattm.data;

import com.android.volley.*;
import com.android.volley.toolbox.JsonArrayRequest;
import com.android.volley.toolbox.JsonObjectRequest;
import com.example.admin.servicechattm.R;
import com.example.admin.servicechattm.ServiceChatApp;
import com.example.admin.servicechattm.model.User;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class UserRequest {

    private static final String MESSAGE_REQUEST_URL = "/Users";

    public static final String ERROR_CONNECTION = "Connection";
    public static final String ERROR_UNKNOWN = "Unknown";
    public static final String ERROR_UNAUTHORIZED = "Unauthorized";

    public interface ResponseListener {
        void onSuccessfulUserFetch(User user);
        void onSuccessfulUserFetch(List<User> user);
        void onError(String error);
    }

    public static JsonObjectRequest create(String username, final String token, final ResponseListener listener) {
        String fullRequestUrl = ServiceChatApp.getContext().getResources().getString(R.string.api_base_url)+ MESSAGE_REQUEST_URL + "/" + username;
        return new JsonObjectRequest(Request.Method.GET, fullRequestUrl, null,
            new Response.Listener<JSONObject>() {
                @Override
                public void onResponse(JSONObject response) {
                    try{
                        listener.onSuccessfulUserFetch(User.fromJsonObject(response));
                    } catch (JSONException e){
                        listener.onError(ERROR_UNKNOWN);
                    }
                }
            },
            new Response.ErrorListener() {
                @Override
                public void onErrorResponse(VolleyError error){
                    handleMessageError(error.networkResponse.statusCode, listener);
                }
            }
        ) {
            @Override
            public Map<String, String> getHeaders() {
                Map<String, String> map = new HashMap<>();
                map.put("Authorization", token);
                return map;
            }
        };
    }

    public static JsonArrayRequest create(final String token, final ResponseListener listener) {
        String fullRequestUrl = ServiceChatApp.getContext().getResources().getString(R.string.api_base_url)+ MESSAGE_REQUEST_URL;
        return new JsonArrayRequest(Request.Method.GET, fullRequestUrl, null,
                new Response.Listener<JSONArray>() {
                    @Override
                    public void onResponse(JSONArray response) {
                        try{
                            listener.onSuccessfulUserFetch(User.fromJsonArray(response));
                        } catch (JSONException e){
                            listener.onError(ERROR_UNKNOWN);
                        }
                    }
                },
                new Response.ErrorListener() {
                    @Override
                    public void onErrorResponse(VolleyError error){
                        handleMessageError(error.networkResponse.statusCode, listener);
                    }
                }
        ) {
            @Override
            public Map<String, String> getHeaders() {
                Map<String, String> map = new HashMap<>();
                map.put("Authorization", token);
                return map;
            }
        };
    }

    private static void handleMessageError(int statusCode, ResponseListener listener){
        switch (statusCode){
            case 403:
                listener.onError(ERROR_UNAUTHORIZED);
                break;
            default:
                listener.onError(ERROR_CONNECTION);
        }

    }
}
