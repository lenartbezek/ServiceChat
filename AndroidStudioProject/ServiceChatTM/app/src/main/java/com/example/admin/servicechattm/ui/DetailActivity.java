package com.example.admin.servicechattm.ui;

import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.widget.TextView;

import com.example.admin.servicechattm.R;

/**
 * Definira activity ki se zgodi ko klikneš na nek element v recycler view-u in ga celega prikaže
 */

public class DetailActivity extends AppCompatActivity {
    private static final String BUNDLE_EXTRAS = "BUNDLE_EXTRAS";
    private static final String EXTRA_MSG = "EXTRA_QUOTE";
    private static final String EXTRA_NAME = "EXTRA_ATTR";


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_detail);

        Bundle extras = getIntent().getBundleExtra(BUNDLE_EXTRAS);

        ((TextView)findViewById(R.id.twMessage)).setText(extras.getString(EXTRA_MSG));
        ((TextView)findViewById(R.id.twMessageAuthor)).setText(extras.getString(EXTRA_NAME));
    }
}