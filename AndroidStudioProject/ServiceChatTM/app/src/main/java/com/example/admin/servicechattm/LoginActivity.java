package com.example.admin.servicechattm;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.EditText;

import android.widget.FrameLayout;
import com.android.volley.RequestQueue;
import com.android.volley.toolbox.Volley;
import com.example.admin.servicechattm.data.LoginRequest;

public class LoginActivity extends AppCompatActivity implements LoginRequest.ResponseListener {

    private EditText etUsername, etPassword;
    private FrameLayout overlay;
    private Button bLogin;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.activity_login);

        etUsername = (EditText) findViewById(R.id.etUsername);
        etPassword = (EditText) findViewById(R.id.etPassword);
        overlay = (FrameLayout) findViewById(R.id.loadingOverlay);
        bLogin = (Button) findViewById((R.id.bLogin));

        loginFromSavedToken();

        bLogin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                loginFromForm();
            }
        });
    }

    private void loginFromForm(){
        final String username = etUsername.getText().toString();
        final String password = etPassword.getText().toString();

        RequestQueue queue = Volley.newRequestQueue(LoginActivity.this);
        queue.add(LoginRequest.create(username, password, this));
        overlay.setVisibility(View.VISIBLE);
    }

    private void loginFromSavedToken(){
        String token;
        try {
            token = Auth.getToken();
        } catch (Auth.TokenNotFoundException e) {
            return; // Not logged in
        }
        RequestQueue queue = Volley.newRequestQueue(LoginActivity.this);
        queue.add(LoginRequest.create(token, this));
        overlay.setVisibility(View.VISIBLE);
    }

    @Override
    public void onSuccess() {
        Intent intent = new Intent(LoginActivity.this, MessageActivity.class);
        LoginActivity.this.startActivity(intent);
        finish();
    }

    @Override
    public void onError(String error) {
        overlay.setVisibility(View.INVISIBLE);
        switch (error){
            case LoginRequest.ERROR_CONNECTION:
                // TODO: Make toast
                break;
            case LoginRequest.ERROR_USER_NOT_FOUND:
                etUsername.setError(getResources().getString(R.string.error_usernotfound));
                break;
            case LoginRequest.ERROR_INVALID_PASSWORD:
                etPassword.setError(getResources().getString(R.string.error_invalidpassword));
                break;
            case LoginRequest.ERROR_INVALID_CREDENTIALS:
                // TODO: Make toast
                break;
        }
    }
}
