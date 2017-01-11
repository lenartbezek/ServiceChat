package com.example.admin.servicechattm.model;

import org.joda.time.DateTime;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.text.ParseException;
import java.util.*;

public class Message {


    private static LinkedList<Message> messages = new LinkedList<>();

    public int id;
    public String username;
    public String text;
    public DateTime time;

    public static Message fromJsonObject(JSONObject object) throws JSONException, ParseException {
        Message message = new Message();
        message.id = object.getInt("Id");
        message.username = object.getString("Username");
        message.text = object.getString("Text");
        message.time = DateTime.parse(object.getString("Time")); // Example: 2016-12-30T22:42:49.493Z
        return message;
    }

    public static List<Message> fromJsonArray(JSONArray array) throws JSONException, ParseException {
        LinkedList<Message> list = new LinkedList<>();
        for(int i = array.length() -1; i >= 0; i--){
            list.add(fromJsonObject(array.getJSONObject(i)));
        }
        return list;
    }

    public static void set(List<Message> newMessages){
        messages.clear();
        messages.addAll(newMessages);
    }

    public static void set(Message newMessage){
        messages.add(0, newMessage);
    }

    public static Message get(int id){
        for (Message message : messages) {
            if (message.id == id) return message;
        }
        return null;
    }

    /**
     * @return A list of all messeges.
     */
    public static List<Message> getAll(){
        return messages;
    }

    public static int count(){
        return messages.size();
    }
}
