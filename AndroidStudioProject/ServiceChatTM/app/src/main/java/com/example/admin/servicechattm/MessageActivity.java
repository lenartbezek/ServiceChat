package com.example.admin.servicechattm;

import android.content.DialogInterface;
import android.content.Intent;
import android.content.res.Configuration;
import android.os.Bundle;
import android.os.Handler;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import com.android.volley.RequestQueue;
import com.android.volley.toolbox.Volley;
import com.example.admin.servicechattm.data.MessageRequest;
import com.example.admin.servicechattm.data.UserRequest;
import com.example.admin.servicechattm.model.Message;
import com.example.admin.servicechattm.model.User;

import java.util.List;

public class MessageActivity extends AppCompatActivity implements MessageRequest.ResponseListener, UserRequest.ResponseListener {

    private final static long UPDATE_INTERVAL = 5000;

    private static MessageActivity singleton;
    private String token;

    private RecyclerView recyclerView;
    private MessageAdapter adapter;
    private Handler updateHandler;
    private Runnable updateData;
    private RequestQueue queue;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_message);

        singleton = this;

        if (getResources().getConfiguration().orientation == Configuration.ORIENTATION_LANDSCAPE)
            getSupportActionBar().hide();
        else
            getSupportActionBar().show();

        recyclerView = (RecyclerView) findViewById(R.id.recyclerView);

        // Use a linear layout manager
        final LinearLayoutManager layoutManager = new LinearLayoutManager(this);
        layoutManager.setReverseLayout(true);
        recyclerView.setLayoutManager(layoutManager);

        // Specify an adapter
        adapter = new MessageAdapter();
        recyclerView.setAdapter(adapter);

        // Get auth token
        try {
            token = Auth.getToken();
        } catch (Auth.TokenNotFoundException e) {
            handleAuthorizationError();
            return;
        }

        queue = Volley.newRequestQueue(MessageActivity.this);

        // Fetch users
        queue.add(UserRequest.create(token, MessageActivity.this));

        // Periodically update messages
        updateHandler = new Handler();

        updateData = new Runnable() {
            @Override
            public void run() {
                queue.add(MessageRequest.create(token, MessageActivity.this));
                updateHandler.postDelayed(this, UPDATE_INTERVAL);
            }
        };

        updateHandler.post(updateData);
    }

    @Override
    public void onDestroy(){
        super.onDestroy();
        if (updateHandler != null)
            updateHandler.removeCallbacks(updateData);
    }

    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);

        if (newConfig.orientation == Configuration.ORIENTATION_LANDSCAPE)
            getSupportActionBar().hide();
        else
            getSupportActionBar().show();
    }

    private void handleAuthorizationError(){
        updateHandler.removeCallbacks(updateData);
        Auth.deleteToken();
        buildErrorAlert("auth error", "");
    }

    private void handleConnectionError(){
        updateHandler.removeCallbacks(updateData);
        Auth.deleteToken();
        buildErrorAlert("connection error", "");
    }

    private void buildErrorAlert(String title, String message){
        final AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setTitle(title);
        builder.setMessage(message)
                .setCancelable(false)
                .setPositiveButton(getResources().getString(R.string.action_ok), new DialogInterface.OnClickListener() {
                    public void onClick(@SuppressWarnings("unused") final DialogInterface dialog, @SuppressWarnings("unused") final int id) {
                        startActivity(new Intent(MessageActivity.this, LoginActivity.class));
                        finish();
                    }
                });
        final AlertDialog alert = builder.create();
        alert.show();
    }

    @Override
    public void onSuccessfulMessageFetch(List<Message> messages) {
        Message.set(messages);
        for (Message m : messages){
            if (User.get(m.username) == null)
                queue.add(UserRequest.create(m.username, token, MessageActivity.this));
        }
        adapter.notifyDataSetChanged();
    }

    @Override
    public void onSuccessfulUserFetch(User user) {
        User.set(user);
        adapter.notifyDataSetChanged();
    }

    @Override
    public void onSuccessfulUserFetch(List<User> users) {
        User.set(users);
        adapter.notifyDataSetChanged();
    }

    @Override
    public void onError(String error) {
        switch (error){
            case MessageRequest.ERROR_UNAUTHORIZED:
                handleAuthorizationError();
                break;
            default:
                handleConnectionError();
                break;
        }
    }
}