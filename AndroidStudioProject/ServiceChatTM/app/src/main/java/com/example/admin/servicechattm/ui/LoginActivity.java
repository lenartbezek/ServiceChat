package com.example.admin.servicechattm.ui;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;

import android.widget.ProgressBar;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.toolbox.Volley;
import com.example.admin.servicechattm.R;
import com.example.admin.servicechattm.model.LoginRequest;

import com.example.admin.servicechattm.model.User;
import org.json.JSONException;
import org.json.JSONObject;

public class LoginActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        final EditText etUsername = (EditText) findViewById(R.id.etUsername);
        final EditText etPassword = (EditText) findViewById(R.id.etPassword);
        final ProgressBar progressBar = (ProgressBar) findViewById(R.id.loginProgressbar);
        final Button bLogin = (Button) findViewById((R.id.bLogin));

        bLogin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                final String username = etUsername.getText().toString();
                final String password = etPassword.getText().toString();

                LoginRequest.ResponseListener listener = new LoginRequest.ResponseListener() {
                    @Override
                    public void onSuccess(User user) {
                        progressBar.setVisibility(View.INVISIBLE);
                        Intent intent = new Intent(LoginActivity.this, MessageActivity.class);
                        LoginActivity.this.startActivity(intent);
                    }

                    @Override
                    public void onError(String error) {
                        progressBar.setVisibility(View.INVISIBLE);
                        switch (error){
                            case LoginRequest.ERROR_CONNECTION:
                                break;
                            case LoginRequest.ERROR_USER_NOT_FOUND:
                                etUsername.setError(getResources().getString(R.string.error_usernotfound));
                                break;
                            case LoginRequest.ERROR_INVALID_PASSWORD:
                                etPassword.setError(getResources().getString(R.string.error_invalidpassword));
                                break;
                        }
                    }
                };

                RequestQueue queue = Volley.newRequestQueue(LoginActivity.this);
                queue.add(LoginRequest.create(username, password, listener));
                progressBar.setVisibility(View.VISIBLE);
            }
        });

    }
}
