package com.example.admin.servicechattm.model;

import com.example.admin.servicechattm.MessageActivity;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.Hashtable;
import java.util.LinkedList;
import java.util.List;

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

    public static List<User> fromJsonArray(JSONArray array) throws JSONException {
        LinkedList<User> list = new LinkedList<>();
        for(int i = 0; i < array.length(); i++){
            list.add(fromJsonObject(array.getJSONObject(i)));
        }
        return list;
    }

    public static void set(List<User> newUsers){
        users.clear();
        for (User u : newUsers)
            users.put(u.username, u);
    }

    public static void set(User newUser){
        users.put(newUser.username, newUser);
    }

    public static User get(String username){
        if (users.containsKey(username)){
            return users.get(username);
        } else {
            return null;
        }
    }

    public static void setMe(User user){
        users.put(user.username, user);
        me = user;
    }

    public static User getMe(){
        return me;
    }

    public static Hashtable<String, User> getAll(){
        return users;
    }

}
