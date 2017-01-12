package com.example.admin.servicechattm.data;

import com.android.volley.*;
import com.android.volley.toolbox.JsonArrayRequest;
import com.android.volley.toolbox.JsonObjectRequest;
import com.example.admin.servicechattm.R;
import com.example.admin.servicechattm.ServiceChatApp;
import com.example.admin.servicechattm.model.Message;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.text.ParseException;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class SendRequest {

    private static final String MESSAGE_REQUEST_URL = "/Send";

    public static final String ERROR_CONNECTION = "Connection";
    public static final String ERROR_UNKNOWN = "Unknown";
    public static final String ERROR_UNAUTHORIZED = "Unauthorized";

    public interface ResponseListener {
        void onSuccessfulMessagePost();
        void onError(String error);
    }

    public static JsonObjectRequest create(String text, final String token, final ResponseListener listener) {
        String fullRequestUrl = ServiceChatApp.getContext().getResources().getString(R.string.api_base_url)+ MESSAGE_REQUEST_URL;
        JSONObject body = new JSONObject();
        try {
            body.put("Text", text);
        } catch (JSONException e) {
            e.printStackTrace();
        }
        return new JsonObjectRequest(Request.Method.POST, fullRequestUrl, body,
                new Response.Listener<JSONObject>() {
                    @Override
                    public void onResponse(JSONObject response) {
                        handleMessageResponse(response, listener);
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

    private static void handleMessageResponse(JSONObject response, ResponseListener listener){
        listener.onSuccessfulMessagePost();
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
