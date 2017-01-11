package com.example.admin.servicechattm.model;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.Hashtable;

public class User {

    private static Hashtable<String, User> users = new Hashtable<>();
    private static User me;

    public String displayName;
    public String username;
    public boolean admin;

    public static User fromJsonObject(JSONObject object) throws JSONException{
        User user = new User();
        user.username = object.getString("Username");
        user.displayName = object.getString("DisplayName");
        user.admin = object.getBoolean("Admin");
        return user;
    }

    public static User get(String username){
        if (users.containsKey(username)){
            return users.get(username);
        } else {
            // TODO: Make request and insert user
            return null;
        }
    }

    public static void setMe(User user){
        users.put(user.username, user);
        me = user;
    }

    public static Hashtable<String, User> getAll(){
        // TODO: Make REST request and update all users
        return users;
    }

}
