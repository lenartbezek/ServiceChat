package com.example.admin.servicechattm.model;

import org.json.JSONException;
import org.json.JSONObject;

public class User {

    public String displayName;
    public String username;
    public boolean admin;

    public static User fromJsonResponse(JSONObject response) throws JSONException{
        User user = new User();
        JSONObject account = response.getJSONObject("Account");
        user.username = account.getString("Username");
        user.displayName = account.getString("DisplayName");
        user.admin = account.getBoolean("Admin");
        return user;
    }

}
