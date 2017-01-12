package com.example.admin.servicechattm.data;

import com.android.volley.*;
import com.android.volley.toolbox.JsonArrayRequest;
import com.example.admin.servicechattm.R;
import com.example.admin.servicechattm.ServiceChatApp;
import com.example.admin.servicechattm.model.Message;
import org.json.JSONArray;
import org.json.JSONException;

import java.text.ParseException;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class MessageRequest {

    private static final String MESSAGE_REQUEST_URL = "/Messages";

    public static final String ERROR_CONNECTION = "Connection";
    public static final String ERROR_UNKNOWN = "Unknown";
    public static final String ERROR_UNAUTHORIZED = "Unauthorized";

    public interface ResponseListener {
        void onSuccessfulMessageFetch(List<Message> messages);
        void onError(String error);
    }

    public static JsonArrayRequest create(final String token, final ResponseListener listener) {
        String fullRequestUrl = ServiceChatApp.getContext().getResources().getString(R.string.api_base_url)+ MESSAGE_REQUEST_URL;
        return new JsonArrayRequest(Request.Method.GET, fullRequestUrl, null,
            new Response.Listener<JSONArray>() {
                @Override
                public void onResponse(JSONArray response) {
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

    private static void handleMessageResponse(JSONArray response, ResponseListener listener){
        try{
            listener.onSuccessfulMessageFetch(Message.fromJsonArray(response));
        } catch (JSONException e){
            listener.onError(ERROR_UNKNOWN);
        } catch (ParseException e) {
            listener.onError(ERROR_UNKNOWN);
        }
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
