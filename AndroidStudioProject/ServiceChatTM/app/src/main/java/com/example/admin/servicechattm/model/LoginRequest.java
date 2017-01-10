package com.example.admin.servicechattm.model;

import com.android.volley.AuthFailureError;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyLog;
import com.android.volley.toolbox.StringRequest;

import org.json.JSONObject;

import java.io.UnsupportedEncodingException;
import java.util.HashMap;
import java.util.Map;

public class LoginRequest extends StringRequest{
    private static final String LOGIN_REQUEST_URL = "http://is-servicechat.azurewebsites.net/api/Login";


    public LoginRequest(String username, String password, Response.Listener<String> listener) {
        super(Request.Method.POST, LOGIN_REQUEST_URL, listener, null);
    }

    @Override
    public String getBodyContentType() {
        return "application/json; charset=utf-8";
    }

    @Override
    public byte[] getBody() throws AuthFailureError {
        try {
            JSONObject jsonBody = new JSONObject();
            jsonBody.put("Username", "admin");
            jsonBody.put("Password", "Geslo.01");
            final String requestBody = jsonBody.toString();
            return requestBody == null ? null : requestBody.getBytes("utf-8");
        } catch (Exception e) {
            VolleyLog.wtf("error");
            return null;
        }
    }
}
