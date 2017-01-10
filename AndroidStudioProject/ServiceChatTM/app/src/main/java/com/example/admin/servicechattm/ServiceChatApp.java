package com.example.admin.servicechattm;

import android.app.Application;
import android.content.Context;

/**
 * Wrapper za dostopanje do contexta iz statičnih helperjev.
 */
public class ServiceChatApp extends Application {
    private static ServiceChatApp instance;

    public static ServiceChatApp getInstance(){
        return instance;
    }

    public static Context getContext(){
        return instance.getApplicationContext();
    }

    @Override
    public void onCreate() {
        instance = this;
        super.onCreate();
    }
}
