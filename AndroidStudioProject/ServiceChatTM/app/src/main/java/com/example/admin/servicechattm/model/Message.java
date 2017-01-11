package com.example.admin.servicechattm.model;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.LinkedList;
import java.util.List;

public class Message {

    // ISO-8601 time format. Example: 2016-12-30T22:42:49.493Z
    private static DateFormat timeFormat = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'");

    private static LinkedList<Message> messages = new LinkedList<>();

    public int id;
    public String username;
    public String text;
    public Date time;

    public static Message fromJsonObject(JSONObject object) throws JSONException, ParseException {
        Message message = new Message();
        message.id = object.getInt("Id");
        message.username = object.getString("Username");
        message.text = object.getString("Text");
        message.time = timeFormat.parse(object.getString("Time"));
        return message;
    }

    public static List<Message> fromJsonArray(JSONArray array) throws JSONException, ParseException {
        LinkedList<Message> list = new LinkedList<>();
        for(int i = 0; i < array.length(); i++){
            list.add(fromJsonObject(array.getJSONObject(i)));
        }
        return list;
    }

    public static Message get(int id){
        for (int i = 0; i < messages.size(); i++){
            if (messages.get(i).id == id) return messages.get(i);
        }
        // TODO: Make request and insert message to list
        return null;
    }

    /**
     * @return A list of all messeges.
     */
    public static List<Message> getAll(){
        // TODO: Get all messages and create list
        return messages;
    }

    /**
     * @return A list of all messages with only the new ones updated.
     */
    public static List<Message> getNew(){
        if (messages.size() < 1)
            return getAll();

        // TODO: Get all messages since last message and append to list
        Message lastMessage = messages.get(messages.size() - 1);

        return messages;
    }
}
